@smoke

Feature: ProductLibrary

A short summary of the feature

@User
Scenario: show the ProductLibrary page
    Given the user navigates to 'cost-models/products'
    Then the table shows a list of products
