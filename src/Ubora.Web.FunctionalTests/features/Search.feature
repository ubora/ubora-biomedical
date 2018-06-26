Feature: Search
    As a user 
    I want to search for projects

Background:
    Given I go to Home page
        And I clicked on the element "span=Projects"

Scenario: I am on Search page
    Then I expect the title of the page "Biomedical device projects - UBORA"

Scenario: I search for existing project
    When I set value "Test" to the element "#Title"
        And I click on the element "button=search"
    Then I expect the element "h5=Test title" is visible
        And I expect the title of the page "Biomedical device projects - UBORA"

Scenario: I search for non-existing project
    When I set value "Ubora Land" to the element "#Title"
        And I click on the element "button=search"
        And I expect the title of the page "Biomedical device projects - UBORA"

Scenario: Signed in user searches for existing project
    When I sign in as user
        And I click on the element "span=Projects"
        And I click on the element "a*=All projects"
        And I set value "Est" to the element "#Title"
        And I click on the element "button=search"
    Then I expect the element "h5=Test title" is visible
        And I expect the title of the page "Biomedical device projects - UBORA"


