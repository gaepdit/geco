# Annual Permit Fees Calculations

**Emissions fee** `NUMCALCULATEDFEE` = Sum of VOC/PM/NOx/SO2 tons `INTVOCTONS + INTPMTONS + INTSO2TONS + INTNOXTONS` * Emissions fee rate `NUMFEERATE`

---

**Part 70 Fee** `NUMPART70FEE` = MAX(Part 70 minimum fee, Emissions fee)

**SM Fee** `NUMSMFEE` = SM fee rate *(if applicable)*

**NSPS Fee** `NUMNSPSFEE` = NSPS fee rate *(if applicable)*

**Maintenance Fee** `MaintenanceFee` = Maintenance fee rate *(if Part 70)*

**Fee subtotal** = Part 70 Fee + SM Fee + NSPS Fee + Maintenance Fee *(Part 70 Fee or SM Fee will be zero)*

---

**Admin Fee** `NUMADMINFEE` = Fee subtotal * Days delayed * Admin fee rate

**Total Fee** `NUMTOTALFEE` = Fee subtotal (Part 70 Fee + SM Fee + NSPS Fee + Maintenance Fee) + Admin Fee *(Part 70 Fee or SM Fee will be zero)*
