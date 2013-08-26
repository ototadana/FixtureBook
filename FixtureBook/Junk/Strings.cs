
namespace XPFriend.Junk
{
    /// <summary>
    /// 文字列関連ユーティリティ。
    /// </summary>
    public sealed class Strings
    {
        private Strings() { }

        /// <summary>
        /// 指定された文字列が null または長さゼロであれば true を返す。
        /// </summary>
        /// <param name="text">調べる文字列。</param>
        /// <returns>null または長さゼロであれば true</returns>
        public static bool IsEmpty(string text)
        {
            return text == null || text.Length == 0;
        }
    }
}
