using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

using Grpc.Core;
using MagicOnion;
using MagicOnion.Client;
using MagicOnion.Unity;

using Gateway.Protocol;
using Gateway.Protocol.Table;

using Entry.Protocol;

public class GatewayService : MonoBehaviour
{
    [SerializeField] private string entryTarget;

    private GrpcChannelx channel;
    [SerializeField] private string targetAddress;
    [SerializeField] private int targetPort;
    

    private IGatewayDispatcher gatewayDispatcher;
    public IGatewayDispatcher GatewayDispatcher
    {
        set => gatewayDispatcher = value;
        get => gatewayDispatcher.WithDeadline(DateTime.UtcNow.AddSeconds(requestTimeout));
    }

    [SerializeField] private int entryRequestTimeout = 5;
    [SerializeField] private int connectionTimeout = 5;
    [SerializeField] private int requestTimeout = 5;
    
    private long userNo = 0L;

    protected T BuildRequest<T>(T req) where T : class, IREQ, new()
    {
        Debug.Log($"<color=green> >>> {typeof(T).Name}</color> : {JsonUtility.ToJson(req)}");
        req.Header = new REQ_Header()
        {
            UserNo = userNo
        };
        return req;
    }
    
    private void Awake() 
    {
        GrpcChannelProviderHost.Initialize(new DefaultGrpcChannelProvider(new []
        {
            new ChannelOption("grpc.keepalive_time_ms", 5000),        
            new ChannelOption("grpc.keepalive_timeout_ms", 5 * 1000),
        }));
    }

    public IEnumerator Connect()
    {
        Debug.Log("[Gateway] Connect Start");
        
        Debug.Log("[Gateway] Entry Request");
        //yield return TargetServerRegistry();
        var entryTask = TargetServerRegistry();        
        while(!entryTask.IsCompleted)
            yield return null;

        Debug.Log("[Gateway] Channel Connect");
        //yield return ConnectChannel();
        var channelTask = ConnectChannel();
        while(!channelTask.IsCompleted)
            yield return null;

        Debug.Log("[Gateway] Gateway Create");
        ConnectGateway();

        Debug.Log("[Gateway] Connect End");
    }

    public async Task TargetServerRegistry()
    {
        if(string.IsNullOrEmpty(entryTarget))
        {			
            Debug.LogError($"entry target setting is null or offline mode");
            return;
        }
        
        Debug.Log($"Connect to EntryServer: {entryTarget}");
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(entryTarget))
        {
            request.timeout = entryRequestTimeout;

            var operation = request.SendWebRequest();
            
            while(!operation.isDone)
                await Task.Yield();
            
            if (request.result != UnityWebRequest.Result.Success)
            {					
                Debug.LogError($"entry server connect is failed : {request.error}");					
            }
            else
            {
                string serverInfo = request.downloadHandler.text;
                var info = JsonUtility.FromJson<RetrieveResult>(serverInfo);

                var uri = new System.Uri(info.Server.Url);
                targetAddress = uri.Host;
                targetPort = uri.Port;
            }
        }

        await Task.Delay(1);
    }

    public async Task ConnectChannel()
    {        
        if(channel == null)
        {            
            channel = GrpcChannelx.ForAddress($"http://{targetAddress}:{targetPort}");            
        }
                            
        try
        {
            await channel.ConnectAsync(DateTime.UtcNow.AddSeconds(connectionTimeout));
        }
        catch(Exception ex)
        {
            Debug.LogError("channel connect error - " + ex.Message);            
        }        

        await Task.Delay(1);
    }

    public void ConnectGateway()
    {
        GatewayDispatcher = MagicOnionClient.Create<IGatewayDispatcher>(channel);
    }

    public async Task<RES_RetrieveAllMasterData> ReqRetrieveAllMasterData()
    {
        try
        {
            var res = await GatewayDispatcher.RetrieveAllMasterDataAsync(BuildRequest(new REQ_RetrieveAllMasterData
            {
                VersionMap = new Dictionary<string, long>(),
            }));

            return res;
        }
        catch(Exception ex)
        {
            Debug.LogError("[Gateway] ReqRetrieveAllMasterData error - " + ex.Message);
            return null;
        }
    }
}