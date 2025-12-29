@smoke
Feature: LandedCosts

Landed costs features

@User
Scenario: Can view the landed costs
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When the user changes the date range from January 2023 to April 2023
    And they change the results-tab to 'Landed Costs'
    And they change the price calculation method to Median
    Then the landed costs results shows the following values
        | Title                   | DataTestId         | Value        |
        | Total shipment          | total-shipment     | 1000.00/UNIT |
        | Shipping costs per unit | shipping-cost-unit | 10.00/UNIT   |
        | Landed costs per unit   | landed-cost-unit   | 34.47/UNIT   |

@User
Scenario: Can view the landed costs table
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When they change the results-tab to 'Landed Costs'
    Then the whats the price table shows the following results
        | CostComponent         | Percentage | Costs |
        | DirectMaterials       | 26         | 6     |
        | DirectLabour          | 9          | 2     |
        | ManufacturingOverhead | 7          | 1     |
        | CostOfSales           | 44         | 10    |
        | GsaAndOtherExpenses   | 45         | 10    |
        | ProfitBeforeTax       | 10         | 2     |
        | Total                 | 100        | 24    |
        | ShippingCost          | 31         | 7     |
        | TotalLandedCost       | 131        | 31    |


@User
Scenario: Can view the landed costs with calculation method median
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When they change the results-tab to 'Landed Costs'
    And they change the price calculation method to Median
    Then the whats the price table shows the following results
        | CostComponent         | Percentage | Costs |
        | DirectMaterials       | 26         | 6     |
        | DirectLabour          | 9          | 2     |
        | ManufacturingOverhead | 7          | 1     |
        | CostOfSales           | 44         | 10    |
        | GsaAndOtherExpenses   | 45         | 10    |
        | ProfitBeforeTax       | 10         | 2     |
        | Total                 | 100        | 24    |
        | ShippingCost          | 41         | 10    |
        | TotalLandedCost       | 141        | 34    |


@User
Scenario: Can view the landed costs with calculation method Max
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When they change the results-tab to 'Landed Costs'
    And they change the price calculation method to Max
    Then the whats the price table shows the following results
        | CostComponent         | Percentage | Costs |
        | DirectMaterials       | 26         | 6     |
        | DirectLabour          | 9          | 2     |
        | ManufacturingOverhead | 7          | 1     |
        | CostOfSales           | 44         | 10    |
        | GsaAndOtherExpenses   | 45         | 10    |
        | ProfitBeforeTax       | 10         | 2     |
        | Total                 | 100        | 24    |
        | ShippingCost          | 62         | 15    |
        | TotalLandedCost       | 162        | 39    |