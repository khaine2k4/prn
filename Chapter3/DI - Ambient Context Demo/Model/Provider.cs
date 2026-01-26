using Demo_DI_MethodInjection.Model;

namespace Demo_DI_AmbientContext.Model
{
    // Ambient Context
    public abstract class DepartmentProvider
    {
        private static DepartmentProvider current;

        public static DepartmentProvider Current
        {
            get
            {
                if (current == null)
                {
                    current = new DefaultDepartmentProvider();
                }
                return current;
            }
            set
            {
                current = value ?? new DefaultDepartmentProvider();
            }
        }

        public virtual Department Department { get; }
    }

    // Default Provider
    public class DefaultDepartmentProvider : DepartmentProvider
    {
        public override Department Department => new Engineering();
    }

    // Custom Provider
    public class MarketingProvider : DepartmentProvider
    {
        public override Department Department => new Marketing();
    }
}
