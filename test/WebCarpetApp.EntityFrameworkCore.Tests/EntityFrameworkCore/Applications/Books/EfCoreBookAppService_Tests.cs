using WebCarpetApp.Books;
using Xunit;

namespace WebCarpetApp.EntityFrameworkCore.Applications.Books;

[Collection(WebCarpetAppTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<WebCarpetAppEntityFrameworkCoreTestModule>
{

}