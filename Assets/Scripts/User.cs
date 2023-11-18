using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Sabanishi.AddressableTest
{
    public class User:MonoBehaviour
    {
        [SerializeField] private AssetReference testPath;
        [SerializeField] private Image testImage;
        private void Start()
        {
            UniTask.Void(async () =>
            {
                await FirebaseInitializer.Initialize(this.GetCancellationTokenOnDestroy());
                Debug.Log("Firebaseの初期化に成功しました");

                var image = await AddressableLoader.Load<Sprite>(testPath);
                testImage.sprite = image;
                
                //5秒待機
                await UniTask.Delay(5000, cancellationToken: this.GetCancellationTokenOnDestroy());
                
                AddressableLoader.Release<Sprite>(testPath);
            });
        }
    }
}