﻿using AutoMapper;
using LeaveManagement.Infrastructure;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _employeeRepository = _unitOfWork.Employee;
            _mapper = mapper;
        }
        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
           var employee = await _employeeRepository.GetByIdAsync(id);

            return _mapper.Map<DTO.EmployeeDTO>(employee);
        }

        public async Task<List<EmployeeDTO>> GetEmployeesAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
           var employees = await _employeeRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
           return _mapper.Map<List<DTO.EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO> UpdateEmployeeAsync(EmployeeDTO employee)
        {
            var employeeEntity = _mapper.Map<Entity.Employee>(employee);
            await _employeeRepository.UpdateAsync(employeeEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.EmployeeDTO>(employeeEntity);
        }
        public async Task<int> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.FindAsync(id);
            await _employeeRepository.DeleteAsync(employee);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<EmployeeDTO> AddEmployeeByAsync(EmployeeDTO employee)
        {
            var employeeEntity = _mapper.Map<Entity.Employee>(employee);
            if (employeeEntity.User != null)
            {
                employeeEntity.User.Password = "experion@123";
            }
            await _employeeRepository.AddAsync(employeeEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.EmployeeDTO>(employeeEntity);
        }

        public async Task<List<EmployeeDTO>> GetEmployeeByDepartmentId(int id)
        {
            var employees = await _employeeRepository.GetEmployeeByDepartmentId(id);
            return _mapper.Map<List<DTO.EmployeeDTO>>(employees);
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesByManagerId(int id)
        {
            var employees = await _employeeRepository.GetAllEmployeesByManagerId(id);
            return _mapper.Map<List<DTO.EmployeeDTO>>(employees);
        }
    }
}
