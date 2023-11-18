using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace Sabanishi.AddressableTest
{
    /// <summary>
    /// Firebaseの初期化を行うクラス
    /// </summary>
    public static class FirebaseInitializer
    {
        /// <summary>
        /// Firebaseを初期化する
        /// </summary>
        public static async UniTask Initialize(CancellationToken token)
        {
            DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            
            //このタイミングでキャンセルされていたら処理を中断して例外を投げる
            token.ThrowIfCancellationRequested();

            if (dependencyStatus != DependencyStatus.Available)
            {
                Debug.LogError($"Firebaseの初期化に失敗しました: {dependencyStatus}");
            }
        }
    }
}