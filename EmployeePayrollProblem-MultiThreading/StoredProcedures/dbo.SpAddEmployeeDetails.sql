
create procedure dbo.SpAddEmployeeDetails
	@EmpName	varchar(150),				
	@StartDate	date,		
	@Gender	char(1),	
	@PhoneNo	bigint,			
   @Address varchar(150),
    @Department	varchar(150),
   @BasicPay float,
	@Deductions	float,	
	@TaxablePay float,
	@Income_Tax	float,
	@NetPay	float
	as begin
	Insert into employee_payroll values(@EmpName,@StartDate,@Gender,@PhoneNo,@Address,@Department,@BasicPay,@Deductions,@TaxablePay,@Income_Tax,@NetPay)
	End