﻿namespace webapi;

public record GameCreateDto(string CreatedBy, string Title, string Description, int RoundTime, int Expiration, string Guid = "");