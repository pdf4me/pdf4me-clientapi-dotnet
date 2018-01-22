# Pdf4me.Client - the C# library for the Pdf4me Saas API

The Pdf4me Client API is a .NET library which connects to its highly scalable SaaS cloud service with many functionalities 
to solve your document and PDF requirements. The SaaS API provides expert functionality to convert, optimize, compress, 
produce, merge, split, ocr, enrich, archive, print documents and PDF's.

Feature | Description 
------------ | ------------- 
[**Optimize**](https://dev-dev.pdf4me.com/#operation/Optimize) | PDF's can often be optimized by removing structural redundancy. This leads to much smaller PDF's.
**Merge** | Multiple PDF's can be merged into single optimized PDFs.
**Split** | A PDF can be splitted into multiple PDF's.
**Extract** | From a PDF extract multiple pages into a new document.
**OCR** | Create a searchable OCR Document out of your scans or images.
**Images** | Extract images from your document, can be any type of document.
**Create Pdf/A** | Create a archive conform PDF/A including xmp Metadata.
**Convert to PDF** | Convert your documents from any format to a proper PDF document.
**Stamp** | Stamp your document with text or images.



<a name="frameworks-supported"></a>
## Frameworks supported
- .NET 4.5.2 or later

<a name="dependencies"></a>
## Dependencies
- [Newtonsoft](https://www.nuget.org/packages/Newtonsoft.Json/) - 10.0.0 or later
- [Microsoft.IdentityModel](https://www.nuget.org/packages/Microsoft.IdentityModel.Clients.ActiveDirectory/) - 3.7.13 or later

The Pdf4me Client Api Library can be downloaded from [NuGet] (https://docs.nuget.org/consume/installing-nuget):
```
Install-Package Pdf4me.Client
```

<a name="getting-started"></a>
## Getting Started
To get started get an Api-Key and Password from our developers page [dev@pdf4me] (https://dev-dev.pdf4me.com).

The Api-Key/Password is required to Authenticate with OAuth2. The Pdf4me Client Api provides you already 
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

            Pdf4meClient.Instance.Init("sample-not-working-key-aaaaaaa", "*********");
         
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

The Api Documentation can be found on our developers page [dev@pdf4me] (https://dev-dev.pdf4me.com).