# Status Code

`FSLK_ADMIN_STATUS`

|   ID | STRIAIPDESC                               | STRGECODESC                                    | STRDMUDESC                                                    |
|-----:|-------------------------------------------|------------------------------------------------|---------------------------------------------------------------|
|    0 | Unknown                                   | Unknown                                        | Unknown                                                       |
|    1 | Added to the Annual Fee System            | Potentially Subject to Fees for                | Initial Insert into FS_Admin                                  |
|    2 | Added to the Annual Fee Mailout           | Potentially Subject to Fees for                | Initial Insert into FS_Mailout                                |
|    3 | Facility Enrolled in the Fee System       | Fee System Available for                       | Update FS_Admin and Insert into FS_FeeData                    |
|    4 | Facility Mailed a Fee Letter              | Fee letter Mailed and Fee System available for | Update FS_Admin.strMailout, Sent FS_Admin.datMailoutSent      |
|    5 | GECO User has updated the Fee Contact     | In Progress - Please Complete the Fees for     | Insert into FS_ContactInfo                                    |
|    6 | GECO User has update the Fee Calculations | In Progress - Please Complete the Fees for     | Update to FS_FeeData                                          |
|    7 | GECO User Has updated the Signature Page  | In Progress - Please Complete the Fees for     | Update to FS_FeeData                                          |
|    8 | GECO User has reported for the Fee Year   | Emissions Fees have been reported for          | Update to FS_Admin.intsubmittal and Insert into FS_FeeInvoice |
|    9 | Partial Payment                           | Custom                                         | Invoice to Transactions Balance <> 0                          |
|   10 | Paid in Full                              | Custom                                         | Invoice and Transactions Balance = 0                          |
|   11 | Out of Balance                            | Custom                                         | Transactions to Invoice > 0                                   |
|   12 | Collections Ceased due to Audit           | Custom                                         | Collections Ceased due to Audit                               |
