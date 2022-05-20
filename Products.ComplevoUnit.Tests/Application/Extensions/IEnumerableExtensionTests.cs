using FluentAssertions;
using Products.Complevo.Application.Core.Extensions;
using Products.Complevo.Domain.Models;
using Products.Complevo.Unit.Tests.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Products.Complevo.Unit.Tests.Application.Extensions
{
    public class IEnumerableExtensionTests
    {
        [Fact]
        public void IsNotNullAndHasItems_IfHasElements_MustValidate()
        {
            var enumerable = new ProductBuilder().Build(10);
            enumerable.IsNotNullAndHasItems().Should().BeTrue();
            enumerable.IsNullOrEmpty().Should().BeFalse();
        }

        [Fact]
        public void IsNotNullAndHasNoItems_MustValidate()
        {
            var enumerable = new ProductBuilder().Build(0);
            enumerable.IsNotNullAndHasItems().Should().BeFalse();
            enumerable.IsNullOrEmpty().Should().BeTrue();
        }

        [Fact]
        public void IsNullAndHasNoItems_MustValidate()
        {
            IEnumerable<Product> enumerable = default;
            enumerable.IsNotNullAndHasItems().Should().BeFalse();
            enumerable.IsNullOrEmpty().Should().BeTrue();
        }
    }
}
