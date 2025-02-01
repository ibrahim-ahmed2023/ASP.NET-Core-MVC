﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts.Interfaces.PersonsInterface
{
    /// <summary>
    /// Represents business logic for manipulating Perosn entity
    /// </summary>
    public interface IPersonsDeleterService
    {

        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="PersonID">PersonID to delete</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePerson(Guid? personID);

    }
}
