﻿using TaskoMask.Application.Core.Commands;
using System.ComponentModel.DataAnnotations;
using TaskoMask.Domain.Share.Helpers;
using TaskoMask.Domain.Share.Resources;

namespace TaskoMask.Application.Workspace.Projects.Commands.Models
{
    public abstract class ProjectBaseCommand : BaseCommand
    {
        public ProjectBaseCommand(string name, string description,string organizationId)
        {
            Name = name;
            Description = description;
            OrganizationId = organizationId;
        }


        [StringLength(DomainConstValues.Project_Name_Max_Length, MinimumLength = DomainConstValues.Project_Name_Min_Length, ErrorMessageResourceName = nameof(DomainMessages.Length_Error), ErrorMessageResourceType = typeof(DomainMessages))]
        [Required(ErrorMessageResourceName = nameof(DomainMessages.Required), ErrorMessageResourceType = typeof(DomainMessages))]
        public string Name { get;  }


        [StringLength(DomainConstValues.Project_Name_Max_Length, MinimumLength = DomainConstValues.Project_Name_Min_Length, ErrorMessageResourceName = nameof(DomainMessages.Length_Error), ErrorMessageResourceType = typeof(DomainMessages))]
        [Required(ErrorMessageResourceName = nameof(DomainMessages.Required), ErrorMessageResourceType = typeof(DomainMessages))]
        public string Description { get;  }

        [Required(ErrorMessageResourceName = nameof(DomainMessages.Required), ErrorMessageResourceType = typeof(DomainMessages))]
        public string OrganizationId { get;  }
    }
}