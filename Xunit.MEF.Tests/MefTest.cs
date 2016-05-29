using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xunit.MEF.Tests
{
    [Export]
    [RunWith(typeof(IocTestClassCommand))]
    [DependencyResolverBootstrapper(typeof(MefDependencyResolverBootstrapper))]
    public class MefTest : IDisposable
    {
        private readonly IComputable _computer;

        [ImportingConstructor]
        public MefTest(IComputable computer)
        {
            this._computer = computer;
        }

        [Fact]
        public void Add()
        {
            var add = _computer.Add(1, 2);
            Assert.Equal(3, add);
        }

        [Fact(DisplayName = "Sub")]
        public void Sub()
        {
            var sub = _computer.Sub(1, 2);
            Assert.Equal(0, sub);
        }

        [Fact]
        public void Mul(IComputable computer)
        {
            var mul = computer.Mul(1, 2);
            Assert.Equal(2, mul);
        }

        public void Div()
        {
            var div = _computer.Div(1, 2);
            Assert.Equal(0, div);
        }

        public void Dispose()
        {

        }
    }

    public interface IComputable
    {
        int Add(int i, int j);
        int Sub(int i, int j);
        int Mul(int i, int j);
        int Div(int i, int j);
    }

    [Export(typeof(IComputable))]
    public class Computer : IComputable
    {
        public int Add(int i, int j)
        {
            return i + j;
        }

        public int Sub(int i, int j)
        {
            return i - j;
        }

        public int Mul(int i, int j)
        {
            return i * j;
        }

        public int Div(int i, int j)
        {
            return i / j;
        }
    }
}
