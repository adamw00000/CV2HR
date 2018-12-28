Feature: Accessing the job offer details
       In order to know more about a job offer
       As a user
       I want to see its details

@JobOfferController
Scenario: Job offer 0 details
       Given when there are following offers in the database
	   | Id | CompanyId | Created    | ValidUntil | Description         | JobTitle     | Location | UserId | SalaryFrom | SalaryTo |
	   | 0  | 1         | 2012-01-26 | 2020-01-26 | the best job ever   | Programmer   | Poland   | user1  |            |          |
	   | 1  | 1         | 2012-01-30 | 2020-01-30 | the best job ever 2 | Programmer 2 | Poland   | user1  |            |          |
	   | 2  | 2         | 2012-01-01 |            | the worst job ever  | Teacher      | Poland   | user2  |            |          |
       When I want to see details of job offer 0
       Then I get following offer on the screen
	   | Id | CompanyId | Created    | ValidUntil | Description         | JobTitle     | Location | UserId | SalaryFrom | SalaryTo |
	   | 0  | 1         | 2012-01-26 | 2020-01-26 | the best job ever   | Programmer   | Poland   | user1  |            |          |

Scenario: Job offer 1 details
       Given when there are following offers in the database
	   | Id | CompanyId | Created    | ValidUntil | Description         | JobTitle     | Location | UserId | SalaryFrom | SalaryTo |
	   | 0  | 1         | 2012-01-26 | 2020-01-26 | the best job ever   | Programmer   | Poland   | user1  |            |          |
	   | 1  | 1         | 2012-01-30 | 2020-01-30 | the best job ever 2 | Programmer 2 | Poland   | user1  |            |          |
	   | 2  | 2         | 2012-01-01 |            | the worst job ever  | Teacher      | Poland   | user2  |            |          |
       When I want to see details of job offer 1
       Then I get following offer on the screen
	   | Id | CompanyId | Created    | ValidUntil | Description         | JobTitle     | Location | UserId | SalaryFrom | SalaryTo |
	   | 1  | 1         | 2012-01-30 | 2020-01-30 | the best job ever 2 | Programmer 2 | Poland   | user1  |            |          |

Scenario: Job offer 2 details
       Given when there are following offers in the database
	   | Id | CompanyId | Created    | ValidUntil | Description         | JobTitle     | Location | UserId | SalaryFrom | SalaryTo |
	   | 0  | 1         | 2012-01-26 | 2020-01-26 | the best job ever   | Programmer   | Poland   | user1  |            |          |
	   | 1  | 1         | 2012-01-30 | 2020-01-30 | the best job ever 2 | Programmer 2 | Poland   | user1  |            |          |
	   | 2  | 2         | 2012-01-01 |            | the worst job ever  | Teacher      | Poland   | user2  |            |          |
       When I want to see details of job offer 2
       Then I get following offer on the screen
	   | Id | CompanyId | Created    | ValidUntil | Description         | JobTitle     | Location | UserId | SalaryFrom | SalaryTo |
	   | 2  | 2         | 2012-01-01 |            | the worst job ever  | Teacher      | Poland   | user2  |            |          |

Scenario: Job offer 3 details
       Given when there are following offers in the database
	   | Id | CompanyId | Created    | ValidUntil | Description         | JobTitle     | Location | UserId | SalaryFrom | SalaryTo |
	   | 0  | 1         | 2012-01-26 | 2020-01-26 | the best job ever   | Programmer   | Poland   | user1  |            |          |
	   | 1  | 1         | 2012-01-30 | 2020-01-30 | the best job ever 2 | Programmer 2 | Poland   | user1  |            |          |
	   | 2  | 2         | 2012-01-01 |            | the worst job ever  | Teacher      | Poland   | user2  |            |          |
       When I want to see details of job offer 3
       Then I get NotFound error