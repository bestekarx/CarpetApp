using Xunit;

namespace WebCarpetApp.EntityFrameworkCore;

[CollectionDefinition(WebCarpetAppTestConsts.CollectionDefinitionName)]
public class WebCarpetAppEntityFrameworkCoreCollection : ICollectionFixture<WebCarpetAppEntityFrameworkCoreFixture>
{

}
