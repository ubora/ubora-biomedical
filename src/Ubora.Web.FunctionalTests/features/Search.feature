Feature: Search
    As a user 
    I want to search for projects

Background:
    Given I go to Home page
        And I clicked on the element "span=Search"

Scenario: I seach for existing project
    When I set value "Test" to the element "#Title"
        And I click on the element "button=Search"
    Then I expect the element "h4=Test title" is visible
        And I expect the element "h4=TestProject" is visible
        And I expect the title of the page "Search - UBORA"

Scenario: I search for non-existing project
    When I set value "Ubora Land" to the element "#Title"
        And I click on the element "button=Search"
        And I expect the title of the page "Search - UBORA"

Scenario: I click Home button
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click Sign in/sign up
    When I click on the element "#SignInSignUp"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: I click Search
    When I click on the element "span=Search"
    Then I expect the title of the page "Search - UBORA"

Scenario: Signed in user searches for existing project
    When I sign in as user
        And I click on the element "span=Search"
        And I set value "Est" to the element "#Title"
    Then I expect the element "h4=Test title" is visible
        And I expect the element "h4=TestProject" is visible
        And I expect the title of the page "Search - UBORA"


