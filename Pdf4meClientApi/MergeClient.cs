using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4meClient
{
    public partial class MergeClient
    {

        public async Task<byte[]> MergeAsync(byte[] doc1, byte[] doc2)
        {

            var req = new Pdf4meClient.Merge()
            {
                Documents = new System.Collections.ObjectModel.ObservableCollection<Document>() {

                new Document()
                {
                    DocData = doc1
                },
                new Document()
                {
                    DocData = doc2
                }
                },
               
            };

            var res = await MergeAsync(req);

            if (res != null && res.Document != null)
            {
                return res.Document.DocData;
            }

            return null;
        }

    }
}
