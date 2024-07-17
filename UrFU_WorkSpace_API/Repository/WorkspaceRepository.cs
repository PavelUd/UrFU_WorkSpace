using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class WorkspaceRepository(UrfuWorkSpaceContext context) : BaseRepository<Workspace>(context), IWorkspaceRepository
{
    
}