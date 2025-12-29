Feature: Manage Products

Managing products

@User
Scenario: Create a product
    Given the user navigates to 'cost-models/products'
    When they create a product with name 'Test Create Product'
    Then the product is created
    And the product has name 'Test Create Product'

@User
@PrepareProduct
Scenario: Edit a product, changes with autosave
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When they change the name to 'Test Edit Product'
    And they change the description to 'Test Description'
    Then the product has name 'Test Edit Product'
    And the product has description 'Test Description'

@User
@PrepareProduct
Scenario: Can delete a product
    Given the user navigates to 'cost-models/products'
    And they search for prepared product
    When they delete the product
    Then the product is deleted

@User
@PrepareProduct
Scenario: Add a single commodity to a product
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user adds the commodity 'Gold'
    Then the direct-materials table should have the following materials
        | Commodity | NetWeight | WeightComposition |
        | Gold      | 1.00      | 100.00%           |

@User
@PrepareProduct
Scenario: Add a multiple commodity to a product
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user adds the commodity 'Gold' and changes the weight to 3
    And the user adds the commodity 'Silver' from 'China'
    Then the direct-materials table should have the following materials
        | Commodity | NetWeight | WeightComposition |
        | Gold      | 3.00      | 75.00%            |
        | Silver    | 1.00      | 25.00%            |
    And the total netweight is '4.00'

@User
@PrepareProduct
@PrepareCompleteProduct
Scenario: Add a component to a product
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user adds the prepared component with multiplier 2
    Then the direct-materials table should have the prepared component
        | NetWeight | WeightComposition |
        | 6.00      | 100.00%           |
    And the total netweight is '6.00'

@User
@PrepareProduct
Scenario: Can select a industry with a region
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user selects the industry 'Breweries' with region 'Belgium'
    Then the product has industry 'Breweries' from 'Belgium'

@User
@PrepareProduct
Scenario: Can select a custom industry with a region
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user selects the custom industry 'Mineral Industry (DO NOT DELETE THIS INDUSTRY)' with region 'Netherlands'
    Then the product has industry 'Mineral Industry (DO NOT DELETE THIS INDUSTRY)' from 'Netherlands'

@User
@PrepareProduct
@PrepareCompleteProduct
Scenario: Can mark a product as complete
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user selects the industry 'Breweries' with region 'Belgium'
    And the user adds the commodity 'Silver' from 'China'
    And the user marks the product as complete
    Then the product is marked as complete

@User
@PrepareProduct
@PrepareCompleteProduct
Scenario: Can select a currency
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When they change the date-control currency to 'USD' and select 'US Dollar'
    Then the product has currency 'US Dollar (USD)'