﻿namespace TaskoMask.Services.Monolith.Application.Workspace.Owners.Commands.Models
{
    public class UpdateOwnerProfileCommand : OwnerBaseCommand
    {
        public UpdateOwnerProfileCommand(string id, string displayName, string email)
              : base(id,displayName, email)
        {

        }

    }
}
