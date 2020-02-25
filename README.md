# [Pdf4me.Client](https://developer.pdf4me.com/docs/api/getting-started/) - the C# library for the Pdf4me Saas API

The Pdf4me Client API is a .NET library which connects to its highly scalable SaaS cloud service with many functionalities 
to solve your document and PDF requirements. The SaaS API provides expert functionality to convert, optimize, compress, 
produce, merge, split, ocr, enrich, archive, rotate, protect, validate, repair, read barcodes, print documents and PDF's.

Feature | Description 
------------ | ------------- 
[**Optimize**](https://developer.pdf4me.com/docs/api/basic-functionality/optimize/) | PDF's can often be optimized by removing structural redundancy. This leads to much smaller PDF's.
[**Merge**](https://developer.pdf4me.com/docs/api/basic-functionality/merge-pdfs/) | Multiple PDF's can be merged into single optimized PDFs.
[**Split**](https://developer.pdf4me.com/docs/api/basic-functionality/split-pdf/) | A PDF can be splitted into multiple PDF's.
[**Extract**](https://developer.pdf4me.com/docs/api/basic-functionality/extract-pdf/) | From a PDF extract multiple pages into a new document.
[**OCR**](https://developer.pdf4me.com/docs/api/basic-functionality/ocr/) | Create a searchable OCR Document out of your scans or images.
[**Images**](https://developer.pdf4me.com/docs/api/basic-functionality/create-image/) | Extract images from your document, can be any type of document.
[**Create Pdf/A**](https://developer.pdf4me.com/docs/api/basic-functionality/pdfa/) | Create a archive confhttps://developer.pdf4me.com/docs/api/basic-functionality/pdfa/)orm PDF/A including xmp Metadata.
[**Convert to PDF**](https://developer.pdf4me.com/docs/api/basic-functionality/convert-to-pdf/) | Convert your documents from any format to a proper PDF document.
[**Stamp**](https://developer.pdf4me.com/docs/api/basic-functionality/stamp/) | Stamp your document with text or images.
[**Rotate**](https://developer.pdf4me.com/docs/api/basic-functionality/rotate-pdf/) | Rotates pages in your document.
[**Protect**](https://developer.pdf4me.com/docs/api/basic-functionality/protect/) | Protects or Unlocks your document with given password.
[**Validation**](https://developer.pdf4me.com/docs/api/basic-functionality/validate/) | Validate your document for PDF/A compliance.
[**Repair**](https://developer.pdf4me.com/docs/api/basic-functionality/repair/) | Repairs your document.
[**Barcode**](https://developer.pdf4me.com/docs/api/basic-functionality/barcode/) | Reads all types of barcode embedded in document or creates them


<a name="frameworks-supported"></a>
## Frameworks supported
- .NET 4.5.2 or later
- .NET Core 2.0 or later

<a name="dependencies"></a>
## Dependencies
- [Newtonsoft](https://www.nuget.org/packages/Newtonsoft.Json/) - 11.0.2 or later

The Pdf4me Client Api Library can be downloaded from [NuGet](https://docs.nuget.org/consume/installing-nuget):
```
Install-Package Pdf4me.Client
```

<a name="getting-started"></a>
## Getting Started
To get started get a Token by dropping us an [email](mailto:support-dev@pdf4me.com) or registering in our [portal](https://portal.pdf4me.com/).

The Token is required for Authentication. The Pdf4me Client Api provides you already 
with the necessary implementation. You need only to get an instance for the Pdf4meClient as shown in the sample below.

```csharp
using Pdf4me.Client;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Pdf4meClient.Instance.Init("sample-not-working-token-aaaaaaa");
         
            var req = new Optimize()
            {
                Document = new Document()
                {
                    DocData = File.ReadAllBytes(@"c:\test.pdf"),
                    Name = "test.pdf",
                },

                OptimizeAction = new OptimizeAction()
                {
                    Profile = OptimizeActionProfile.Max,
                    UseProfile = true,                    
                }
            };

            var optimizedDocument = Pdf4meClient.Instance.PdfClient.OptimizeAsync(req).GetAwaiter().GetResult();

        }
    }
}
```


<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

The Api Documentation can be found on our developers page [https://developer.pdf4me.com](https://developer.pdf4me.com).

#### PDF4me Consumer
Those who are looking for PDF4me online tool can find it at [PDF4me.com](https://pdf4me.com/). 
