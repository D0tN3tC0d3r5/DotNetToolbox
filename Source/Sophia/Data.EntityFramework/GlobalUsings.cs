global using System;
global using System.Collections;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq.Expressions;

global using DotNetToolbox.AI.Agents;
global using DotNetToolbox.AI.Common;
global using DotNetToolbox.Domain.Models;

global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using Sophia.Data.Chats;
global using Sophia.Data.Personas;
global using Sophia.Data.Providers;
global using Sophia.Data.Tools;
global using Sophia.Data.Users;
global using Sophia.Data.World;
global using Sophia.Models.Chats;
global using Sophia.Models.Personas;
global using Sophia.Models.Providers;
global using Sophia.Models.Tools;
global using Sophia.Models.Users;
global using Sophia.Models.Worlds;

global using static System.Diagnostics.Ensure;
global using static System.StringComparison;
