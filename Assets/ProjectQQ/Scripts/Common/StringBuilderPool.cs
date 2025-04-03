using System.Text;
using UnityEngine.Pool;

namespace QQ
{
    public static class StringBuilderPool
    {
        private static ObjectPool<StringBuilder> pool = new ObjectPool<StringBuilder>(
            createFunc: () => new StringBuilder(),  // StringBuilder 생성, 초기 버퍼 사이즈 default : 16
            actionOnGet: sb => sb.Clear(),  // 가져올 때 초기화
            actionOnRelease: sb => sb.Clear(),  // 반환시 초기화
            actionOnDestroy: sb => sb.Clear(), // 삭제 시 초기화
            maxSize: 1000  // 최대 크기
        );

        public static string Get(params string[] text)
        {
            var sb = pool.Get();

            foreach (var t in text)
            {
                sb.Append(t);
            }

            string result = sb.ToString();

            pool.Release(sb);

            return result;
        }
    }
}
