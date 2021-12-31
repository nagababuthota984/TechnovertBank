using AutoMapper;
using System;
using TechnovertBank.API.ApiModels;
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.API
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AccountViewModel, Account>();
            CreateMap<Account, AccountViewModel>();
            CreateMap<BankViewModel, Bank>();
            CreateMap<Bank, BankViewModel>();
            CreateMap<Employee, Employee>();
            CreateMap<Employee, Employee>();
            CreateMap<CurrencyViewModel, Currency>();
            CreateMap<Currency, CurrencyViewModel>();
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<TransactionViewModel, Transaction>();
            CreateMap<Transaction, TransactionViewModel>();

            CreateMap<CreateAccountModel, CustomerViewModel>();
            CreateMap<UpdateAccountModel, CustomerViewModel>();
        }
    }
}
