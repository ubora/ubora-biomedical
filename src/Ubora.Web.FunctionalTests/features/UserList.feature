Feature: Members page
    As a developer
    I want the demo app have correct page

Background:
    Given I go to the website "/UserList"

Scenario: Get the title of Ubora 
    Then I expect the title of the page "- UBORA"

Scenario: I click a Ubora logo
    When I click on the element "#header-logo"
    Then I expect the title of the page "Welcome - UBORA"

