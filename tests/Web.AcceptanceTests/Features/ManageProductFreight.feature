Feature: Manage Product Freight

Managing product freight

@User
@PrepareProduct
Scenario: Can add a product freight lane
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user opens the freight lane search modal and selects the freight lane 'Test Freight Lane (DO NOT DELETE THIS FREIGHT LANE)'
    Then the product has freight lane with name 'Test Freight Lane (DO NOT DELETE THIS FREIGHT LANE)' and price 1000

@User
@PrepareProduct
Scenario: Can delete a product freight lane
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user opens the freight lane search modal and selects the freight lane 'Test Freight Lane (DO NOT DELETE THIS FREIGHT LANE)'
    And they delete the freight lane with name 'Test Freight Lane (DO NOT DELETE THIS FREIGHT LANE)'
    Then the product does not have a freight lane

@User
@PrepareProduct
Scenario: Can edit a product freight lane
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user opens the freight lane search modal and selects the freight lane 'Test Freight Lane (DO NOT DELETE THIS FREIGHT LANE)'
    And they edit the number of units to 100
    Then the product has a freight lane with the number of units of 100