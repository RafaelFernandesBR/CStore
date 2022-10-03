namespace LojaAc
{
    public static class FText
    {

        public static string GetWords(String str, int Length)
        {
            String strRet = "";
            if (Length > 0)
            {
                String[] strs = str.Split(' ');
                int i = 0;
                if (strs.Length < Length)
                {
                    Length = strs.Length;
                }
                while (i < Length)
                {
                    if (i == 0)
                    {
                        strRet += string.Format("{0}", strs[i]);
                    }
                    else
                    {
                        strRet += string.Format(" {0}", strs[i]);
                    }
                    i++;
                }

            }

            return strRet;
        }

    }
}