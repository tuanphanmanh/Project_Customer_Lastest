using esign.Auditing;
using esign.Test.Base;
using Shouldly;
using Xunit;

namespace esign.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("esign.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("esign.Auditing.GenericEntityService`1[[esign.Storage.BinaryObject, esign.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.esignuctName.Services.Base.EntityService`6[[CompanyName.esignuctName.Entity.Book, CompanyName.esignuctName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.esignuctName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("esign.Auditing.XEntityService`1[esign.Auditing.AService`5[[esign.Storage.BinaryObject, esign.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[esign.Storage.TestObject, esign.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
