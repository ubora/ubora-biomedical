Feature: Feedback
    As a user 
    I want to send feedback to the development team

Background:
    Given I go to Home page

Scenario: I sign in and send feedback
    When I sign in as user
    And I click on the element "i=chat_bubble"
    Then I expect the element "h2=Feedback" is visible
    When I set value "Is this the feedback you are looking for?" to the element "#feedback"
    And I click on the element "button=Send"
    Then I expect the element "p=Thank you!" is visible
    And I expect the title of the page "Welcome - UBORA"

Scenario: I try to send feedback but cancel it
    When I click on the element "i=chat_bubble"
    Then I expect the element "h2=Feedback" is visible
    When I set value "There is no feedback for you." to the element "#feedback"
    And I click on the element "button=Cancel"
    Then I expect the element "h3=You have no projects." is visible
    And I expect the title of the page "Welcome - UBORA"