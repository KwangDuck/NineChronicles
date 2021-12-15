using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncIO;
using Bencodex.Types;
using Cysharp.Threading.Tasks;
using Lib9c.Renderer;
using Libplanet;
using Libplanet.Action;
using Libplanet.Assets;
using Libplanet.Blockchain;
using Libplanet.Blockchain.Policies;
using Libplanet.Blocks;
using Libplanet.Crypto;
using Libplanet.Net;
using Libplanet.RocksDBStore;
using Libplanet.Store;
using Libplanet.Store.Trie;
using Libplanet.Tx;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Nekoyume.Action;
using Nekoyume.BlockChain.Policy;
using Nekoyume.Helper;
using Nekoyume.L10n;
using Nekoyume.Model.Item;
using Nekoyume.Model.State;
using Nekoyume.Serilog;
using Nekoyume.State;
using Nekoyume.UI;
using NetMQ;
using Serilog;
using Serilog.Events;
using UniRx;
using UnityEngine;
using static Nekoyume.Action.ActionBase;
using NCAction = Libplanet.Action.PolymorphicAction<Nekoyume.Action.ActionBase>;
using NCTx = Libplanet.Tx.Transaction<Libplanet.Action.PolymorphicAction<Nekoyume.Action.ActionBase>>;

namespace Nekoyume.BlockChain
{
    public class DummyAgent : MonoBehaviour, IDisposable, IAgent
    {
        public Subject<long> BlockIndexSubject { get; } = new Subject<long>();

        public long BlockIndex => blocks?.Tip?.Index ?? 0;

        public PrivateKey PrivateKey { get; private set; }

        public Address Address => PrivateKey.PublicKey.ToAddress();

        public BlockPolicySource BlockPolicySource => throw new NotImplementedException();

        public BlockRenderer BlockRenderer { get; private set; } = new BlockRenderer();

        public ActionRenderer ActionRenderer { get; private set; } = new ActionRenderer();

        public int AppProtocolVersion { get; private set; }

        public Subject<BlockHash> BlockTipHashSubject { get; } = new Subject<BlockHash>();

        public BlockHash BlockTipHash => blocks.Tip.Hash;

        private readonly Subject<(NCTx tx, List<NCAction> actions)> _onMakeTransactionSubject =
            new Subject<(NCTx tx, List<NCAction> actions)>();

        public IObservable<(NCTx tx, List<NCAction> actions)> OnMakeTransaction => _onMakeTransactionSubject;

        private readonly ConcurrentQueue<NCAction> _queuedActions = new ConcurrentQueue<NCAction>();

        protected BlockChain<NCAction> blocks;

        public void Dispose()
        {
            // TODO: implementation
            _onMakeTransactionSubject.Dispose();

            BlockRenderHandler.Instance.Stop();
            ActionRenderHandler.Instance.Stop();
            ActionUnrenderHandler.Instance.Stop();
        }

        public IObservable<ActionBase.ActionEvaluation<T>> RequestAction<T>(T gameAction) where T : GameAction
        {
            // TODO: implementation
            var states = new Dictionary<Address, IValue>
            {
                { Address, null }
            }.ToImmutableDictionary();
            var balances = new Dictionary<(Address, Currency), BigInteger>().ToImmutableDictionary();
            var ev = new NCActionEvaluation
            {
                Action = gameAction,
                Signer = Address,
                BlockIndex = BlockIndex,
                OutputStates = new AccountStateDelta(states, balances),
                Exception = null,
                PreviousStates = new AccountStateDelta(states, balances),
            }.ToActionEvaluation();
            //ActionRenderer.ActionRenderSubject.OnNext(ev);

            return Observable.Return(new ActionEvaluation<T>
            {
                Action = gameAction
            });
        }


        public void EnqueueAction(GameAction gameAction)
        {
            Debug.LogFormat("Enqueue GameAction: {0} Id: {1}", gameAction, gameAction.Id);
            // handle game actions

            var states = new Dictionary<Address, IValue>
            {
                { Address, null }
            }.ToImmutableDictionary();
            var balances = new Dictionary<(Address, Currency), BigInteger>().ToImmutableDictionary();
            var ev = new NCActionEvaluation
            {
                Action = gameAction,
                Signer = Address,
                BlockIndex = BlockIndex,
                OutputStates = new AccountStateDelta(states, balances),
                Exception = null,
                PreviousStates = new AccountStateDelta(states, balances),
            }.ToActionEvaluation();
            ActionRenderer.ActionRenderSubject.OnNext(ev);
        }

        public Task<Dictionary<Address, AvatarState>> GetAvatarStates(IEnumerable<Address> addressList)
        {
            throw new NotImplementedException();
        }

        public FungibleAssetValue GetBalance(Address address, Currency currency)
        {
            throw new NotImplementedException();
        }

        public Task<FungibleAssetValue> GetBalanceAsync(Address address, Currency currency)
        {
            throw new NotImplementedException();
        }

        public IValue GetState(Address address)
        {
            return null;
        }

        public Task<IValue> GetStateAsync(Address address)
        {
            return Task.Run(() => GetState(address));
        }

        public IEnumerator Initialize(CommandLineOptions options, PrivateKey privateKey, Action<bool> callback)
        {
            yield return new WaitForEndOfFrame();

            // assign privatekey
            PrivateKey = privateKey;
            // assign appProtocolVersion
            var appProtocolVersion = options.AppProtocolVersion is null
                ? default
                : Libplanet.Net.AppProtocolVersion.FromToken(options.AppProtocolVersion);
            AppProtocolVersion = appProtocolVersion.Version;

            // set agent state
            States.Instance.SetAgentStateAsync(
                GetStateAsync(Address).Result is Bencodex.Types.Dictionary agentDict
                        ? new AgentState(agentDict)
                        : new AgentState(Address)
            );

            // assign gold balance
            States.Instance.SetGoldBalanceState(new GoldBalanceState(Address,
                new FungibleAssetValue()));

            // set state config state
            States.Instance.SetGameConfigState(new GameConfigState());

            // set weekly arena state
            var weeklyAreanaState = new WeeklyArenaState(Address);
            States.Instance.SetWeeklyArenaState(weeklyAreanaState);

            // register block/action renderer
            BlockRenderHandler.Instance.Start(BlockRenderer);
            ActionRenderHandler.Instance.Start(ActionRenderer);
            ActionUnrenderHandler.Instance.Start(ActionRenderer);            

            callback?.Invoke(true);
        }

        public bool IsTxStaged(TxId txId)
        {
            throw new NotImplementedException();
        }

        public void SendException(Exception exc)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTxId(Guid actionId, out TxId txId)
        {
            throw new NotImplementedException();
        }
    }
}
