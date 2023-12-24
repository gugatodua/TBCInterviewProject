﻿using Application.Persons.Commands;
using MediatR;

namespace Application.Persons.CommandHandlers
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.Id);

            _personRepository.Delete(person);
            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}
