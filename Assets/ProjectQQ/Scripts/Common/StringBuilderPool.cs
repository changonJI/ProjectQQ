using System.Text;
using UnityEngine.Pool;

namespace QQ
{
    public static class StringBuilderPool
    {
        private static ObjectPool<StringBuilder> pool = new ObjectPool<StringBuilder>(
            createFunc: () => new StringBuilder(),  // StringBuilder ����, �ʱ� ���� ������ default : 16
            actionOnGet: sb => sb.Clear(),  // ������ �� �ʱ�ȭ
            actionOnRelease: sb => sb.Clear(),  // ��ȯ�� �ʱ�ȭ
            actionOnDestroy: sb => sb.Clear(), // ���� �� �ʱ�ȭ
            maxSize: 1000  // �ִ� ũ��
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
