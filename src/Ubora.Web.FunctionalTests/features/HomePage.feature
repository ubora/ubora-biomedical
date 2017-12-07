Feature: Home page
    As a user 
    I want to click all buttons on Home page

Background:
    Given I go to Home page

Scenario: Go to UBORA home page 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Home button
    When I click on the element "#UboraLogo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Log in button
    When I click on the element "span=Log in"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click I have an idea button
    When I click on the element "a=I have an idea"
    Then I expect the element ".tooltip-inner=You have to log in to create a project!" is visible

Scenario: Click on Community
    When I click on the element "span=Community"
    Then I expect the title of the page "Community - UBORA"

Scenario: Click on Search
    When I click on the element "span=Search"
    Then I expect the title of the page "Search - UBORA"