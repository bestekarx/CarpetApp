using WebCarpetApp.Samples;
using Xunit;

namespace WebCarpetApp.EntityFrameworkCore.Applications;

[Collection(WebCarpetAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<WebCarpetAppEntityFrameworkCoreTestModule>
{

}
