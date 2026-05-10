@smoke
Feature: WhatsThePrice

Calculating product prices

@User
Scenario: Can view whats the price of simple product
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Bottle (DO NOT DELETE THIS PRODUCT)' and open the product details
    When the user changes the date range from January 2023 to April 2023
    And they change the results-tab to "What's the price"
    Then the whats the price table shows the following results
        | CostComponent         | Percentage | Costs |
        | DirectMaterials       | 13         | 0     |
        | DirectLabour          | 17         | 0     |
        | ManufacturingOverhead | 27         | 0     |
        | CostOfSales           | 58         | 0     |
        | GsaAndOtherExpenses   | 36         | 0     |
        | ProfitBeforeTax       | 4          | 0     |
        | Total                 | 100        | 0     |
        
@User
Scenario: Can view whats the price of product with components
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When the user changes the date range from January 2023 to April 2023
    And they change the results-tab to "What's the price"
    Then the whats the price table shows the following results
        | CostComponent         | Percentage | Costs | Price paid | Difference |
        | DirectMaterials       | 27         | 6     | 8          | 1          |
        | DirectLabour          | 9          | 2     | 2          | 0          |
        | ManufacturingOverhead | 7          | 1     | 2          | 0          |
        | CostOfSales           | 45         | 11    | 13         | 2          |
        | GsaAndOtherExpenses   | 44         | 10    | 13         | 2          |
        | ProfitBeforeTax       | 9          | 2     | 3          | 0          |
        | Total                 | 100        | 24    | 30         | 5          |

@User
Scenario: Can view whats the price of product with components - different date range
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When the user changes the date range from January 2023 to February 2023
    And they change the results-tab to "What's the price"
    Then the whats the price table shows the following results
        | CostComponent         | Percentage | Costs | Price paid | Difference |
        | DirectMaterials       | 27         | 6     | 9          | 2          |
        | DirectLabour          | 9          | 2     | 3          | 0          |
        | ManufacturingOverhead | 7          | 1     | 2          | 0          |
        | CostOfSales           | 45         | 10    | 14         | 3          |
        | GsaAndOtherExpenses   | 44         | 10    | 14         | 3          |
        | ProfitBeforeTax       | 9          | 2     | 3          | 0          |
        | Total                 | 100        | 24    | 32         | 8          |

@User
Scenario: Can view latest whats the price of product with components
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When the user changes the date range from January 2023 to April 2023
    And they change the results-tab to "What's the price"
    And they change the price calculation method to Latest
    Then the whats the price table shows the following results
        | CostComponent         | Percentage | Costs | Price paid | Difference |
        | DirectMaterials       | 28         | 6     | 7          | 0          |
        | DirectLabour          | 9          | 2     | 2          | 0          |
        | ManufacturingOverhead | 7          | 1     | 2          | 0          |
        | CostOfSales           | 45         | 11    | 12         | 1          |
        | GsaAndOtherExpenses   | 44         | 10    | 12         | 1          |
        | ProfitBeforeTax       | 9          | 2     | 2          | 0          |
        | Total                 | 100        | 24    | 28         | 3          |

@User
Scenario: Can view the historical product prices comparison
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When the user changes the date range from January 2023 to April 2023
    And they change the results-tab to "Price Discipline"
    Then the price discipline chart shows "purchasing-prices" and "price-discipline" series

@User
Scenario: Can view the direct materials costs in the product currency
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    Then the product direct material costs shows 'EUR'

@User
Scenario: Can view the direct materials costs in a different currency
    Given the user navigates to 'cost-models/products'
    And they search for 'Test Beer (DO NOT DELETE THIS PRODUCT)' and open the product details
    When they change the date-control currency to 'USD' and select 'US Dollar'
    Then the product direct material costs shows 'USD'
    