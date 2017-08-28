Feature: Feedback
    As a user 
    I want to send feedback to the development team

Background:
    Given I go to Home page

Scenario: I sign up and send feedback
    When I click on the element "#SignInSignUp"
    And I sign up as "Major.Dajor@email.com" first name "Major" last name "Dajor"
    And I click on the element "i=chat_bubble"
    Then I expect the element "h2=Feedback" is visible
    When I set value "Is this the feedback you are looking for?" to the element "#feedback"
    And I click on the element "button=Send"
    Then I expect the element "p=Thank you!" is visible
    And I expect the title of the page "Create a profile - UBORA"

Scenario: I try to send feedback but cancel it
    When I click on the element "i=chat_bubble"
    Then I expect the element "h2=Feedback" is visible
    When I set value "There is no feedback for you." to the element "#feedback"
    And I click on the element "button=Cancel"
    Then I expect the element "h3=You have no projects." is visible
    And I expect the title of the page "Welcome - UBORA"