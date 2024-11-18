﻿using BookHub.Models.Account;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BookHub.Contracts.REST.Requests.Account.Roles;

/// <summary>
/// Контракт для параметров запроса по добавлению роли.
/// </summary>
public sealed class AddNewRoleParams
{
    [JsonProperty("name", Required = Required.Always)]
    public required string Name { get; init; }

    [JsonProperty("permissions", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required IReadOnlyCollection<PermissionType> Permissions { get; init; }
}