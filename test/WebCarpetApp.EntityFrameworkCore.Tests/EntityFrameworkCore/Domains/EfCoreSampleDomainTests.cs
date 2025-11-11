using WebCarpetApp.Samples;
using Xunit;

namespace WebCarpetApp.EntityFrameworkCore.Domains;

[Collection(WebCarpetAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<WebCarpetAppEntityFrameworkCoreTestModule>
{

}
