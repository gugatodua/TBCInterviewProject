﻿using Application.CustomExceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Application.Persons.Commands
{
    public class DeletePersonCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer _localizer;

        public DeletePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork, IStringLocalizer localizer)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }
        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var person = await _personRepository.GetPersonDbModelByIdAsync(request.Id);
                
                if (person == null)
                {
                    throw new PersonNotFoundException(System.Net.HttpStatusCode.NotFound, _localizer["PersonNotFound"]);
                }

                _personRepository.Delete(person);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
          
            return Unit.Value;
        }
    }
}
