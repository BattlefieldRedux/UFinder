using System;
using System.Collections.Generic;
using System.IO;

namespace UFinder
{
    public class UFinder
    {
        private const string COPY_EXTENSION = ".bck";
        private readonly string mFilePath;
        private readonly List<Pool> mPool;
        private readonly bool mKeepCopy;
        private readonly bool mCaseSensitive;

        UFinder(UFinderBuilder builder)
        {
            mFilePath = builder.mFilePath;
            mPool = builder.mPool;
            mKeepCopy = builder.mKeepCopy;
            mCaseSensitive = builder.mCaseSensitive;
        }

       public static UFinderBuilder NewBuilder(string filePath, List<Pool> pool)
        {
            return new UFinderBuilder(filePath, pool);
        }


     public    void FindAndReplace()
        {

            string tmpFile = Path.GetTempFileName();
            using (StreamWriter writer = new StreamWriter(tmpFile))
            {


                using (StreamReader reader = new StreamReader(mFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        for (int i = 0; i < mPool.Count; i++)
                        {
                            line = mPool[i].FindAndReplace(line, mCaseSensitive);
                        }
                        writer.WriteLine(line);
                    }
                }
            }

            if (mKeepCopy)
            {
                File.Copy(mFilePath, mFilePath + COPY_EXTENSION);
            }

            File.Delete(mFilePath);
            File.Move(tmpFile, mFilePath);
        }

        public class UFinderBuilder
        {
            internal string mFilePath;
            internal List<Pool> mPool;
            internal bool mCaseSensitive;
            internal bool mKeepCopy;

            internal UFinderBuilder(string filePath, List<Pool> pool)
            {
                mFilePath = filePath;
                mPool = pool;
            }


            public UFinderBuilder CaseSensitive(bool caseSensitive)
            {
                mCaseSensitive = caseSensitive;
                return this;
            }


            public UFinderBuilder KeepCopy(bool keepCopy)
            {
                mKeepCopy = keepCopy;
                return this;
            }

            public UFinder Build() {
                return new UFinder(this);
            }
        }
    }
}
