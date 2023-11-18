using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sabanishi.AddressableTest
{
    /// <summary>
    /// Addressableのロード/解放を行うクラス
    /// </summary>
    public class AddressableLoader
    {
        private Dictionary<AssetReference, object> _loadedAssetDict;
        private static AddressableLoader _instance;

        public static AddressableLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                    _instance._loadedAssetDict = new();
                }

                return _instance;
            }
        }

        /// <summary>
        /// AssetReferenceからアセットをロードする
        /// </summary>
        public static async UniTask<T> Load<T>(AssetReference path)
        {
            if (Instance._loadedAssetDict.TryGetValue(path, out var asset))
            {
                return (T)asset;
            }

            var handle = path.LoadAssetAsync<T>();
            var result = await handle.Task;
            Instance._loadedAssetDict.Add(path, result);
            return result;
        }

        /// <summary>
        /// AssetReferenceからアセットを解放する
        /// </summary>
        public static void Release<T>(AssetReference path)
        {
            if (Instance._loadedAssetDict.TryGetValue(path, out var content))
            {
                Addressables.Release(content);
                Instance._loadedAssetDict.Remove(path);
            }
            else
            {
                Debug.LogError("指定されたアセットは読み込まれていません");
            }
        }

        /// <summary>
        /// AssetReferenceからGameObjectを生成し、指定されたコンポーネントを取得する
        /// </summary>
        public static async UniTask<T> Instantiate<T>(AssetReference path) where T : MonoBehaviour
        {
            var obj = await Instantiate(path);
            if (obj == null) return null;

            if (obj.TryGetComponent<T>(out var component))
            {
                return component;
            }

            Debug.LogError("指定されたコンポーネントがアタッチされていないため、新規にアタッチしました");
            return obj.AddComponent<T>();
        }

        /// <summary>
        /// AssetReferenceからGameObjectを生成する
        /// </summary>
        public static async UniTask<GameObject> Instantiate(AssetReference path)
        {
            var handle = path.InstantiateAsync();
            await handle.Task;
            var obj = handle.Result;
            if (obj == null)
            {
                Debug.LogError("生成に失敗しました");
                return null;
            }

            //Instance破壊時に自動でメモリを解放するためのスクリプトをアタッチする
            obj.AddComponent<AddressableInstantiatedEntity>();
            return obj;
        }
    }
}