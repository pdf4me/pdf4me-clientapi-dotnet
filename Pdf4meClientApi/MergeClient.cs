using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4me.Client
{
    public class MergeClient
    {

        private PdfClient _pdfClient;

        public MergeClient(PdfClient pdfClient)
        {
            _pdfClient = pdfClient;
        }

        public async Task<byte[]> MergeAsync(byte[] doc1, byte[] doc2)
        {

            var req = new Merge()
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

            var res = await _pdfClient.MergeAsync(req);

            if (res != null && res.Document != null)
            {
                return res.Document.DocData;
            }

            return null;
        }

    }
}
