# Annual Permit Fees Calculations

Calculated fee `NUMCALCULATEDFEE` = Sum of VOC/PM/NOx/SO2 tons `INTVOCTONS + INTPMTONS + INTSO2TONS + INTNOXTONS` * Emissions fee rate `NUMFEERATE`

Part 70 Fee `NUMPART70FEE` = MAX(Part 70 fee rate, Calculated fee)

SM Fee `NUMSMFEE` = SM fee rate *(unless Part 70 Fee applies)*

NSPS Fee `NUMNSPSFEE` = NSPS fee rate

Intermediate total fee = Part 70 Fee + SM Fee + NSPS Fee *(Part 70 Fee or SM Fee will be zero)*

Admin Fee `NUMADMINFEE` = Intermediate total fee * Days delayed * Admin fee rate

Total Fee `NUMTOTALFEE` = Intermediate total fee (Part 70 Fee + SM Fee + NSPS Fee) + Admin Fee *(Part 70 Fee or SM Fee will be zero)*
