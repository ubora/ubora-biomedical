Feature: Feedback
    As a user
    I want to send feedback to the development team

Background:
    Given I go to Home page

Scenario: I sign in and send feedback
    When I sign in as user
        And I click on the element "button.feedback-trigger"
    Then I expect the element "h3=Leave feedback" is visible
    When I click on the element "textarea.feedback-input"
        And I click on keys "Is this the feedback you are looking for?"
        And I click on the element "button=Send"
    Then I expect the element "p=Thank you for your feedback! 😃" is visible
        And I expect the title of the page "Welcome - UBORA"