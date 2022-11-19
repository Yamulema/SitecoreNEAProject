
namespace Neambc.Neamb.Feature.Product.Model
{
    public class ActionButtonResult
    {
        public string ActionInner { get; set; }
        public string ActionClick { get; set; }
        public bool HasError { get; set; }
        public Anonymous Anonymous { get; set; }
        public bool HasErrorLink { get; set; }
    }
}