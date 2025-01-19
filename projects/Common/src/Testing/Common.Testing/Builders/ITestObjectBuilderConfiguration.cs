namespace RioScaffolding.OpenSpacePlanner.Common.Testing.Builders;

public interface ITestObjectBuilderConfiguration<TObject>
    where TObject : class
{
    Faker<TObject> Configure(Faker<TObject> faker);
}
