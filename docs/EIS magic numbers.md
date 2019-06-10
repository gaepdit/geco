# Access Code

| EISACCESSCODE | STRDESC                                                 |
|--------------:|---------------------------------------------------------|
|             0 | FI access allowed with edit; EI access allowed, no edit |
|             1 | FI and EI access allowed, both with edit                |
|             2 | FI and EI access allowed, both no edit                  |
|             3 | Facility not in EIS                                     |
|             4 | Facility has no access to FI or EI                      |

# Status Code

| EISSTATUSCODE | STRDESC                  |
|--------------:|--------------------------|
|             0 | Not applicable           |
|             1 | Applicable - not started |
|             2 | In progress              |
|             3 | Submitted                |
|             4 | QA Process               |
|             5 | Complete                 |

# QA Status

| QASTATUSCODE | STRDESC         |
|-------------:|-----------------|
|            1 | QA Started      |
|            2 | FI QA Passed    |
|            3 | EPA Submitted   |
|            4 | FI PRD Passed   |
|            5 | Point QA Passed |

# Opt Out Status

| Code | Reason                   |
|-----:|--------------------------|
|    0 | Not opted out (opted in) |
|    1 | Opted out                |

# Opt Out Reason

| Code | Reason                              |
|-----:|-------------------------------------|
|    1 | Facility did not operate            |
|    2 | Facility emissions below thresholds |
