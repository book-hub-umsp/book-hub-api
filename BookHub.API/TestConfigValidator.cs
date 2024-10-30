using Microsoft.Extensions.Options;

namespace BookHub.API;

[OptionsValidator]
public sealed partial class TestConfigValidator : IValidateOptions<TestConfig>;
