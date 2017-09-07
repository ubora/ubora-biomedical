Feature: Project Applicable Regulations Questionnaire
    As a project member
    I want to be able to start and finish a questionnaire to find out applicable regulations for the project

Background:
    Given I am signed in as user and on first page
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "a=Regulation checklist"

Scenario: Start questionnaire > answer all the questions > see summary
    When I click on the element "button=Start questionnaire"
        # Answer questions until the end of questionnaire
        And I click on the element "button=Yes"
        And I click on the element "button=No"
        And I click on the element "button=Yes"
        And I click on the element "button=No"
        And I click on the element "button=Yes"
        And I click on the element "button=No"
    # Check if a random ISO standard that applies to all projects is visible
    Then I expect the element "a=EN ISO 15223-1:2016" is visible 